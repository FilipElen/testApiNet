﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using garage_app_entities;

namespace DAL.Repositories
{
    public class ProductRepository
    {
        private readonly MyDbContext _context;
        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public void InsertProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public List<Product> GetProducts()
        {
           return _context.Products.ToList();
        }

        public Product FindProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                return product;
            }
            else
            {
                throw new ArgumentException("Product was not found!");
            }
        }
        public Product FindProduct(string name)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name.Equals(name));
            if (product != null)
            {
                return product;
            }
            else
            {
                throw new ArgumentException("Product was not found!");
            }
        }

        public List<Category> GetCategoriesFromProduct(int productId)
        {
            return new List<Category>(_context.Products.Where(p => p.Id == productId).SelectMany(c => c.Categories));
        }

        public List<Product> GetProductsByCategory(string categoryType)
        {
            return new List<Product>(_context.Categories.Where(c => c.Type == categoryType).SelectMany(c => c.Products));
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;

            foreach (Category category in product.Categories)
            {
                _context.Categories.Attach(category);
            }

            // code to delete category from a product (delete entry in join table
            var categoriesFromDb = _context.Products.Where(p => p.Id == product.Id).SelectMany(p => p.Categories);
            List<Category> categoriesToDelete = new List<Category>();
            foreach (Category categoryFromDb in categoriesFromDb)
            {
                if (!product.Categories.Contains(categoryFromDb))
                {
                    categoriesToDelete.Add(categoryFromDb);
                }
            }

            _context.Entry(product).Collection("Categories").Load();
            foreach (Category category in categoriesToDelete)
            {
                product.Categories.Remove(category);
            }


            _context.SaveChanges();
        }
        public void DeleteProduct(int productId)
        {
            Product productFromDb = _context.Products.Find(productId);
            if (productFromDb != null)
            {
                _context.Products.Remove(productFromDb);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Product was not found!");
            }
        }
    }
}
