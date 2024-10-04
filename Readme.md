# 🚀 Supabase C# Integration

This repository demonstrates how to integrate Supabase with C# applications, focusing on querying data from multiple schemas and tables. It serves as a template for connecting to Supabase backends and performing database operations in C# environments.

## 📚 Table of Contents

- [Overview](#-overview)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Setup](#%EF%B8%8F-setup)
- [Usage](#-usage)
- [Integration with Existing Projects](#-integration-with-existing-projects)
- [WinForms Integration Example](#-winforms-integration-example)
- [Data Export](#-data-export)
- [Best Practices](#-best-practices)
- [Contributing](#-contributing)
- [License](#-license)

## 🔍 Overview

This project demonstrates how to:

- Connect to a Supabase backend using C#
- Query data from multiple tables (using ACLED, CIA Factbook, and World Bank data as examples)
- Structure a C# application for Supabase integration
- Export data to CSV or Excel formats

While the example uses specific datasets, the techniques can be applied to any Supabase project.

## 🛠 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/downloads) (optional, for cloning the repository)

## 📥 Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/ali2kan/supabase-csharp-integration.git
   cd supabase-csharp-integration
   ```

2. Install required NuGet packages:
   ```bash
   dotnet add package Supabase
   dotnet add package DotNetEnv
   ```

## ⚙️ Setup

1. Create a `.env` file in the project root with your Supabase credentials:
   ```plaintext
   SUPABASE_URL=your_supabase_url
   SUPABASE_KEY=your_supabase_key
   ```

2. Update the `Program.cs` file with your specific table names and query requirements.

## 🚀 Usage

To run the application:

```bash
dotnet run
```

This will execute the sample queries and display the results in the console.

## 🔗 Integration with Existing Projects

To integrate Supabase into an existing C# project:

1. Install the required NuGet packages (Supabase, DotNetEnv).
2. Add the model classes for your Supabase tables (see `Program.cs` for examples).
3. Initialize the Supabase client:
   ```csharp
   var options = new SupabaseOptions
   {
       AutoRefreshToken = true,
       AutoConnectRealtime = true
   };
   var client = new Supabase.Client(supabaseUrl, supabaseKey, options);
   await client.InitializeAsync();
   ```
4. Use the client to query your data:
   ```csharp
   var response = await client
       .From<YourModel>()
       .Select()
       .Get();
   ```

## 🖥 WinForms Integration Example

To integrate Supabase querying into a WinForms application:

1. Create a new WinForms project in Visual Studio.
2. Install the Supabase NuGet package.
3. Add a method to initialize the Supabase client:
   ```csharp
   private Supabase.Client _supabaseClient;

   private async Task InitializeSupabaseClient()
   {
       string supabaseUrl = ConfigurationManager.AppSettings["SupabaseUrl"];
       string supabaseKey = ConfigurationManager.AppSettings["SupabaseKey"];

       var options = new SupabaseOptions
       {
           AutoRefreshToken = true,
           AutoConnectRealtime = true
       };

       _supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, options);
       await _supabaseClient.InitializeAsync();
   }
   ```
4. Call this method in your form's constructor or load event:
   ```csharp
   public Form1()
   {
       InitializeComponent();
       InitializeSupabaseClient().Wait();
   }
   ```
5. Create methods to query data and update UI controls:
   ```csharp
   private async Task LoadData()
   {
       var response = await _supabaseClient
           .From<YourModel>()
           .Select()
           .Get();

       dataGridView1.DataSource = response.Models;
   }
   ```
6. Call these methods from button click events or other appropriate UI triggers.

## 📊 Data Export

To export data to CSV:

1. Install the CsvHelper NuGet package:
   ```bash
   dotnet add package CsvHelper
   ```
2. Use the following code to export data:
   ```csharp
   using (var writer = new StreamWriter("output.csv"))
   using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
   {
       csv.WriteRecords(response.Models);
   }
   ```

For Excel export, consider using libraries like EPPlus or ClosedXML.

## 🌟 Best Practices

- Keep sensitive information (like API keys) in environment variables or secure configuration files.
- Use asynchronous methods consistently for better performance.
- Implement proper error handling and logging.
- Use strongly-typed models to represent your database tables.
- Regularly update and maintain your Supabase client and other dependencies.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.