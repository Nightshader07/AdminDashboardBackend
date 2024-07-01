using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AdminDashboard.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AdminDashboard.Repositories
{
    public class EmployeRepository : IEmplyeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employe> GetAll()
        {
            return _context.Employes.ToList();
        }

        public Employe GetByEmail(string email)
        {
            return _context.Employes.FirstOrDefault(e => e.Email == email);
        }

        public Employe Add(Employe employe)
        {
            // Hash the password before storing
            employe.Password = BCrypt.Net.BCrypt.HashPassword(employe.Password);
            _context.Employes.Add(employe);
            _context.SaveChanges();
            return employe;
        }

        public Employe RemoveById(long id)
        {
            var employe = _context.Employes.Find(id);
            if (employe != null)
            {
                _context.Employes.Remove(employe);
                _context.SaveChanges();
            }
            return employe;
        }

        public Employe Update(Employe employe)
        {
            _context.Entry(employe).State = EntityState.Modified;
            _context.SaveChanges();
            return employe;
        }

        public Employe GetById(long id)
        {
            Employe? employe = _context.Employes.Find(id);
            return employe;
        }

        public Employe Authentificate(string email,string password)
        {
            Employe? employe = GetByEmail(email);
            if (employe == null)
            {
                return null; 
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, employe.Password);
            if (isPasswordValid)
            {
                return employe;
            }

            return null;
        }
    }
}