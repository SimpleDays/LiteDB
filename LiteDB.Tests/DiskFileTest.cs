﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using LiteDB.Shell;

namespace LiteDB.Tests
{
    [TestClass]
    public class DiskFileTest : TestBase
    {
       [TestMethod]
       public void DiskFile_Test()
       {
          //var dbname = @"C:\Git\LiteDB\LiteDB.Shell\bin\Debug\disk.db"; // DB.RandomFile();
          var dbname = DB.RandomFile();

         //File.Delete(dbname);

         using (var db = new LiteDatabase(dbname))
          {
             var shell = new LiteShell(db);

             shell.Run("db.col1.insert {_id:10}");
             shell.Run("db.col1.insert {_id:2}");
             shell.Run("db.col1.insert {_id:3}");

             var col1 = db.GetCollection("col1");

             var d = col1.FindAll().First();

             shell.Run("db.col1.insert {_id:4}");
          }
       }
    }
}