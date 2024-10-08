# SynapseOrderProcessing

SynapseOrderProcessing is a C# program designed to fetch medical equipment orders from an API, check their delivery status, send delivery alerts, and update orders through another API. The application is built to be robust and scalable, with a focus on efficient order management.

As I do not know if the company is using .Net 6 or 8, I included syntax for both, and would follow whatever standards the company has laid out.
## Features

- Fetches orders from an API and checks delivery status.
- Sends delivery alerts when an order is delivered.
- Increments delivery notifications.
- Updates orders via another API.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio or any preferred C# IDE

### Installation

1. Clone the repository:
   git clone https://github.com/your-repo/SynapseOrderProcessing.git

## Proposed Improvments

1. Implemented Cancellation Tokens on the asynchronous calls.
2. Added specific error handling for various errors, such as HTTP errors and deserialization errors.
3. Abstracted all the API calls into their own "Manager" classes (e.g., `OrdersManager`, `ItemsManager`) that would be injected into the main processing class. This would also allow better testing of the functions.
4. Used configuration data in an `appsettings.json` file instead of hard-coded URLs for the endpoints.
5. Finished the test suite to ensure comprehensive coverage and reliability.
6. Added more inline comments throughout the code to clarify functionality and improve maintainability.
7. I would have used SeriLog for logging

## Assumtions

I chose to use JSON Deserialzation instead of the Index notation the previous programmer used.
I matched exactly the casing and spelling of all properties that were previously referenced
using a JObject (jObject["property"]

As I don't have the actual JSON contract to verify, another solution would have been
to manually parse the JObject proprety by property.
