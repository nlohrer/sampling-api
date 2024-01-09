# sampling-api
 API for drawing and analyzing random samples 

# Usage
## Running with dotnet run
To run the API locally, you need to have the .NET SDK 8.0 installed:

```bash
git clone https://github.com/nlohrer/sampling-api.git
cd sampling-api/SamplingAPI
dotnet run
```

The app will be available under `http://http://localhost:5120/`. You can access swagger UI under `http://localhost:5120/swagger/index.html`, which also includes openAPI documentation and sample requests to start out with.

## Using the API
The swagger UI endpoint renders all of the openAPI documentation, including examples for each endpoint to get started with. The documentation is generated from xml comments in the code; you can find most of it, in particular the examples, in the `Controllers` directory.
### Estimators
Estimators are available under the endpoints at `/api/estimator`. All estimators expect a JSON array of numbers named `data`, which can be understood as a column of your data table. For example, the `/api/estimator/srs` endpoint expects the following format:
```
{
    "data": [23, 83, 53, 34],
    "withReplacement": false,
    "populationSize": 25,
    "significanceLevel": 5
}
```
Since `data` consists of numbers, you need to find a fitting encoding for nonnumerical variables. For example, if you have the followig data:
```
[true, false, false, true, false]
```
you could represent it like so:
```
[1, 0, 0, 1, 0]
```
which would result in proportion estimation.
### Sampling
You can take samples by providing data to the endpoints under `/api/sample`. To do so, you need to provide a JSON object of arrays similiar to the `data` array that the estimator endpoints use. However, the sampling endpoints accept data of any JSON type, not just numbers. Here is an example:
```
{
    "name": ["Alice", "Bob", "Carol"],
    "age": [30, 28, 48],
    "married": [false, true, true]
}
```
If your data is not in the proper format, you might find an endpoint under `/api/format` that formats your data so you can use it for sampling.
### Sample Size
The endpoints under `/api/samplesize` allow you to get the (minimum) sample size needed to achieve a particular level of certainty for your estimation.
### Formatting
You can use the endpoints at `/api/format` to format your data into the format used by the endpoints for estimation and sampling.