# Cards API

This small cards API was prepared as part of a recruitment process.
Of course, some corners were intentionally cut, as this is a quick demonstration project meant to showcase core skills rather than represent a fully production-ready solution.

## 📦 Libraries used
- FluentValidation (request validation)
- CsvHelper (CSV data parsing)
- Swagger (API documentation)
- CorrelationId (Scope correlation Id)
- Automapper
- xUnit + FluentAssertions + Moq (unit testing)

## 🧠 Technical Decisions

### ❗ Error handling
I've implemented my own little ExecutionResult. Non of exisitng NuGet solutions ever met my standards and I've always end up creating my own. For full production environment I could improve it and put to separate NuGet package etc.
I generally avoid using exceptions to control application flow, as I believe it's better to reserve them for truly exceptional or unexpected situations. Using exceptions for regular logic paths can reduce code readability and performance.

### 📖 CSV Import
On application startup, rules are loaded from a CSV file.
Based on exact business requirements I bet there is better way to store them than in memory storage, but for this small app purpose I thought this is good enough, as I have them easily readable/editable.

### 📝 Response model
I chose to return the action as an object in the response, as this approach aligns better with REST principles and allows for future extensibility.


## 💻 Commands

### 🚀 Runnig app

```bash
dotnet run
```

### 🧪 Running Tests

```bash
dotnet test
```

