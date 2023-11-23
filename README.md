# AcademicHub
The course of Software Engineering Practices Project



# How to start working with the AcademicHub?

## Clone the repository
First of all, you need to clone the repository:

```bash
git clone https://github.com/podkolzzzin/AcademicHub.git
```

## Project setup
Then open the project in JetBrains Rider and run the following command in the console to start Docker:

```bash
docker-compose up -d
```

## Install Entity Framework tools
Next, install the `dotnet ef` tool globally using this command:

```bash
dotnet tool install --global dotnet-ef
```

## Update the database
Navigate to the Data folder:

```bash
cd .\Identity.Data\
```

And use the following command to update the database:

```bash
dotnet ef database update
```

Here what should you see Docker has started up successfully and database was updated.

![image](https://github.com/podkolzzzin/AcademicHub/assets/94047397/c0469c30-beed-447c-ba0e-d1bed468cf78)

After that you are ready to work with the project


