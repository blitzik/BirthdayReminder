﻿using Perst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Services.Persistence
{
    public class StorageFactory
    {
        public const string DATABASE_EXTENSION = "dbs";
        public const string MAIN_DATABASE_NAME = "data";
        public const string MAIN_DATABASE_FILE_NAME = MAIN_DATABASE_NAME + "." + DATABASE_EXTENSION;


        public StorageFactory()
        {
        }


        public Storage OpenConnection(string databaseName)
        {
            Storage db = Perst.StorageFactory.Instance.CreateStorage();
            db.Open(Path.Combine(GetDatabaseDirectoryPath(), string.Format("{0}.{1}", databaseName, DATABASE_EXTENSION)), 15 * 1024 * 1024);

            Root root = db.Root as Root;
            if (root == null) {
                root = new Root(db);
                db.Root = root;
            }

            return db;
        }


        public static string GetDatabaseDirectoryPath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "_birthdayReminder");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
