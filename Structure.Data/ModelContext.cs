﻿namespace Structure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Entity;
    using Structure.Models;

    public class ModelContext : DbContext, Structure.Services.IModelContext
    {
        /// <summary>
        /// Constructs a new instance of a <see cref="ModelContext"/>
        /// </summary>
        public ModelContext() { }

        /// <summary>
        /// Gets a collection of clients from the data store
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets a collection of users from the data store
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Returns a queryable interface for an entity
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns><see cref="IQueryable"/></returns>
        public IQueryable<T> AsQueryable<T>(params string[] includes) where T : Entity
        {
            var query = this.Set<T>().AsQueryable();
            
            if(includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            
            return query;
        }

        /// <summary>
        /// Returns a single entity. Throws an exception if none or more than one found.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="id">The entity key</param>
        /// <returns><see cref="Entity"/></returns>
        public T Get<T>(int id, params string[] includes) where T : Entity
        {
            if (includes == null)
                return this.Set<T>().Single(x => x.Id == id);
            else
                return this.AsQueryable<T>(includes).Single(x => x.Id == id);
        }

        /// <summary>
        /// Returns a single entity or null if not found.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="id">The entity key</param>
        /// <returns><see cref="Entity"/></returns>
        public T Find<T>(int id, params string[] includes) where T : Entity
        {
            if (includes == null)
                return this.Set<T>().Find(id);
            else
                return this.AsQueryable<T>(includes).SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Deletes a single entity
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="id">The entity key</param>
        /// <returns>True if successful</returns>
        public bool Delete<T>(int id) where T : Entity
        {
            var entity = this.Find<T>(id);
            return this.Delete(entity);
        }

        /// <summary>
        /// Deletes a single entity
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>True if successful</returns>
        /// <exception cref="ArgumentNullException" />
        public bool Delete<T>(T entity) where T : Entity
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "Entity can not be null when calling delete(entity)");

            this.Set<T>().Remove(entity);
            return this.SaveChanges() > 0;
        }

        /// <summary>
        /// Persists changes to the data store
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>The updated entity</returns>
        public T Save<T>(T entity) where T : Entity
        {
            if (entity.Id > 0)
            {
                this.Entry(entity).State = System.Data.EntityState.Modified;
                entity.ChangedDate = DateTime.Now;
            }
            else
            {
                this.Set<T>().Add(entity);
            }
            this.SaveChanges();
            return entity;
        }

    }
}
