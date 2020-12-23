# SearchEngineResultsLookup
Look up for the position of URL for Keywords based search.

## Build Prerequisites
Install Yarn: https://classic.yarnpkg.com/en/docs/install/#mac-stable
Install Node and NPM: https://nodejs.org/en/download/
Install dotnet: https://dotnet.microsoft.com/download/dotnet-core/3.1

- Clone Repo
git clone git@github.com:shishirajm/SearchEngineResultsLookup.git

## Building and Running

- Navigate to folder
cd SearchEngineResultsLookup
- Create artefacts
dotnet publish -o ./out -c Release
Note: if it fails first time try again
- Navigate
cd out
- Run
dotnet SearchEngineResultsLookup.dll

Access: https://localhost:5001/

## Building and Running: Developer
- After cloning navigate to
ClientApp inside the solution
- Run command
yarn install
- Open the sln file in visual studio
Usual debugging process by pressing F5

Access: https://localhost:5001/

## Decisions/Short cuts
- I have tried to showcase:
  On Backend
  * Dependency Injection: Autofac
  * Parallel execution of tasks: async await
  * Simple caching: IMemoryCache from Microsoft
  * Sample unit tests: Nunit, NSubstitue
  On Frontend
  * React component, React Hooks with TypeScript
  * Styled components
  * Sample unit tests: Jest and Enzyme
- In parser storing the whole node though we need only the URL, just that if we need the title or something later can be extracted. It can be cleaned up. Currently it wont impact the results if the whole URL is provided.
- I have just added few tests, I know the HTTP/fetch requests on the Backend and Front end can be mocked. There is scope for hundreds of tests cases, both on front end and backend. Only have set up test for few scenarios.
- I have used parallel tasks to query google and bing, I know there are few optimisation needs to be done for number of threads to spawn, but haven't included it here.
- Since the requirements said production quality code, there are lot of things which could have been done or done better like:
  Backend
 * Better error handling
 * Using and configuring the appsettings
 * Standardising the log format
 * Lot more unit tests
  Frontend
 * Could have used debounce
 * Felt redux would be over kill
 * I know UI can be lot better (But this is what happens when developer designs :) )
 * Lot more unit tests
 
 # Addressing Performance
 - At code level, Parallel.foreach and Task.WhenAll can be combined and optimized to get best result querying.
 - Though problem looks small, this can easily grow into full blown application. Instead of 100 results it can go up to thousands with multiple people using it.
 - Caching would first cause the issue as memory chace scale up well beyond certain point. 
 - Use the redis/elasticsearch cache on separate node, which can be used by different nodes in a cluster.
 - Redis/Elasticsearch can be furhter optimised by properly managing indexes.
 - We can also store the data on DB just for back up. Currently queried can be loaded to cache if redis also grows too huge.
 
 # Addressing Availability and Scalability
 - Supporting blue green deployment of instance.
 - Depends on how scalable it has to be and usage, initially we can deploy on better server machines or EC2 istances, but it can only scale up to some point.
 - Code can be run on multiple instances and at mutiple data centres across different regions. This will increase the complexity as well.
 - As mentioned earlier, redis cache and DB would come in handy, so multiple nodes (server instance) can refer to a cache.
 - Redis and Elasticseach available on AWS and Azure has fully managed hosting to make few things easier.
 
 # Addressing Reliability
 - Making sure code is well tested, by adding in better unit tests and integration tests. Test automation.
 - Any code fix should be deployed as soon as possibile. A quick turnaround time with good automation.
 - With caching, there are chances of rendering outdated information. May be often searched data can be queried and update the cache regularly from a separate task, if the results have changed.
 - Addressing the above mentioned, performance and Availability and scalability helps being reliable.
 

