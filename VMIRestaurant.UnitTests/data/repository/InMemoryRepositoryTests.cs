using System;
using System.Collections.Generic;
using VMIRestaurant.common;
using VMIRestaurant.data.repository;
using Xunit;

namespace VMIRestaurant.UnitTests.data.repository
{
    public class InMemoryRepositoryTests
    {
        private class MockEntity : IEntity { public int Id { get; set; } }

        private class MockRepository : InMemoryEntityRepository<MockEntity>
        { public MockRepository(IEnumerable<MockEntity> entities) : base(entities) { } }
        
        [Fact]
        public void gets_entity_by_id_when_entity_exists()
        {
            var entities = new List<MockEntity>{new MockEntity {Id = 1}, new MockEntity {Id = 2}};
            var mockRepository = new MockRepository(entities);

            var entity = mockRepository.FindById(2);
            
            Assert.NotNull(entity);
            Assert.IsType<MockEntity>(entity);
            Assert.Equal(2, entity.Id);
        }

        [Fact]
        public void errors_when_requested_entity_id_does_not_exist()
        {
            var entities = new List<MockEntity>{new MockEntity {Id = 1}, new MockEntity {Id = 2}};
            var mockRepository = new MockRepository(entities);
            
            Assert.Throws<ArgumentException>(() => mockRepository.FindById(3));
        }

        [Fact]
        public void adds_new_entity()
        {
            var mockRepository = new MockRepository(new List<MockEntity>());
            
            Assert.False(mockRepository.Exists(1));
            
            mockRepository.Add(new MockEntity {Id = 1});
            
            Assert.True(mockRepository.Exists(1));
        }

        [Fact]
        public void increments_entity_id_when_new_entity_is_added()
        {
            var entities = new List<MockEntity>{new MockEntity {Id = 8}};
            var newEntity = new MockEntity();
            var mockRepository = new MockRepository(entities);
            
            mockRepository.Add(newEntity);
            
            Assert.Equal(9, newEntity.Id);
            Assert.True(mockRepository.Exists(9));
        }

        [Fact]
        public void errors_when_adding_entity_with_existing_key()
        {
            var entities = new List<MockEntity>{new MockEntity {Id = 1}};
            var mockRepository = new MockRepository(entities);

            Assert.Throws<ArgumentException>(() => mockRepository.Add(new MockEntity {Id = 1}));
        }

        [Fact]
        public void updates_an_existing_entity()
        {
            var entities = new List<MockEntity>{new MockEntity {Id = 1}};
            var mockRepository = new MockRepository(entities);

            mockRepository.Update(1, new MockEntity {Id = 2});
            
            Assert.True(mockRepository.Exists(1));
            Assert.Equal(2, mockRepository.FindById(1).Id);
        }
        
        [Fact]
        public void errors_when_updated_entity_id_does_not_exist()
        {
            var mockRepository = new MockRepository(new List<MockEntity>());

            Assert.Throws<ArgumentException>(() => mockRepository.Update(1, new MockEntity {Id = 1}));
        }

        [Fact]
        public void deletes_an_entity()
        {
            var entities = new List<MockEntity>{new MockEntity {Id = 1}};
            var mockRepository = new MockRepository(entities);

            Assert.True(mockRepository.Exists(1));
            var entity = mockRepository.Delete(1);
            
            Assert.False(mockRepository.Exists(1));
            Assert.Same(entity, entities[0]);
        }

        [Fact]
        public void errors_when_deleted_entity_id_does_not_exist()
        {
            var mockRepository = new MockRepository(new List<MockEntity>());

            Assert.Throws<ArgumentException>(() => mockRepository.Delete(1));
        }
    }
}
