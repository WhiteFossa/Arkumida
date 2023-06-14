// See https://aka.ms/new-console-template for more information

using furtails_importer.Importers;

var mainImporter = new MainImporter();

Task.WaitAll(mainImporter.ImportAsync());