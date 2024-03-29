﻿using System;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Filter;
using SchoolAPI.Infrastructure;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers.API
{
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly DataStore _dataStore;
        private readonly IServer _server;

        public StudentsController(DataStore dataStore, IServer server)
        {
            _dataStore = dataStore;
            _server = server;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var addresses = _server.Features.Get<IServerAddressesFeature>();
            var address = new Uri(addresses.Addresses.First());

            var host = NetworkInterface.GetAllNetworkInterfaces()
              .Where(p => p.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
              .Select(p => p.GetIPProperties())
              .SelectMany(p => p.UnicastAddresses)
              .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
              .FirstOrDefault()?.Address.ToString();

            if (_dataStore.Students != null)
            {
                _dataStore.Students.ForEach(s => s.LastName = host);
                return Ok(_dataStore.Students);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var student = _dataStore.Students.SingleOrDefault(c => c.ID == id);
            if (student != null)
            {
                return Ok(student);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody]Student student)
        {
            _dataStore.Students.Add(student);
            return Created(Request.GetDisplayUrl() + "/" + student.ID, student);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Student student)
        {
            var exisitingStudent = _dataStore.Students.SingleOrDefault(c => c.ID == id);

            if (exisitingStudent == null) return NotFound();

            _dataStore.Students.Remove(exisitingStudent);
            _dataStore.Students.Add(student);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exisitingStudent = _dataStore.Students.SingleOrDefault(c => c.ID == id);

            if (exisitingStudent == null) return NotFound();

            _dataStore.Students.Remove(exisitingStudent);
            return Ok();
        }
    }
}
