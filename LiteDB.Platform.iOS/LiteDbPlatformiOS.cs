﻿using System.Collections.Specialized;
using System.IO;
using LiteDB.Core;
using LiteDB.Interfaces;

namespace LiteDB.Platform.iOS
{
   public class LiteDbPlatformiOS : LiteDbPLatformBase
   {
      public override FileDiskServiceBase CreateFileDiskService(ConnectionString conn, Logger log)
      {
         return new FileDiskServiceDotNet(conn, log);
      }

      public LiteDbPlatformiOS(IEncryptionFactory encryptionFactory, IReflectionHandler reflectionHandler, IFileHandler fileHandler)
         : base(() => encryptionFactory, () => reflectionHandler, () => fileHandler)
      {
         AddNameCollectionToMapper();
      }

      public LiteDbPlatformiOS() : base(() => new EncryptionFactory(),
         () => new ExpressionReflectionHandler(), () => new FileHandler())
      {
         AddNameCollectionToMapper();
      }

      public void AddNameCollectionToMapper()
      {
         BsonMapper.Global.RegisterType(
            nv =>
            {
               var doc = new BsonDocument();

               foreach (var key in nv.AllKeys)
               {
                  doc[key] = nv[key];
               }

               return doc;
            },

            bson =>
            {
               var nv = new NameValueCollection();
               var doc = bson.AsDocument;

               foreach (var key in doc.Keys)
               {
                  nv[key] = doc[key].AsString;
               }

               return nv;
            }
         );
      }
   }

   public class FileHandler : IFileHandler
   {
      public Stream ReadFileAsStream(string filename)
      {
         return File.OpenRead(filename);
      }

      public Stream CreateFile(string file, bool overwritten)
      {
         return new FileStream(file, overwritten ? FileMode.Create : FileMode.CreateNew);
      }
   }

   public class EncryptionFactory : IEncryptionFactory
   {
      public IEncryption CreateEncryption(string password)
      {
         return new SimpleAES(password);
      }

      public byte[] HashSHA1(string str)
      {
         return SimpleAES.HashSHA1(str);
      }
   }
}
