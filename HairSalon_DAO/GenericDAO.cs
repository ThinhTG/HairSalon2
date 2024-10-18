﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_DAO
{
    public class GenericDAO<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericDAO(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public  List<T> GetAll()
        {
            return  _dbSet.ToList();
        }

        public  T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void  Add(T entity)
        {
             _dbSet.Add(entity);
             _context.SaveChanges();
        }

        public void  Update(T entity)
        {
           
            _context.Entry(entity).State = EntityState.Modified;
             _context.SaveChanges();
        }

        public void  Delete(int id)
        {
            var entity =  GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
