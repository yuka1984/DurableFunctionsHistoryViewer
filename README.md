# DurableFunctionsHistoryViewer

nuget
https://www.nuget.org/packages/DurableFunctionsHistoryViewer

"DurableFunctionsHistoryViewer" is an Azure Functions Extension for making it possible to check the history information of Durable Functions by the browser.

This package can not be used with Azure Functions V1.

## Geting started

* You install this package in the Durable Functions project.
* https://{hostname}/runtime/webhooks/dfhv/index/?code={masterhostkey} You access this URL in the browser.
If you were executing with local debugging, you do not need to specify [code]paramater in QueryString.

## option

```json
    "dfhv": {
      "HubName": "TaskHub0001",
      "OffsetHour":  "9" 
    }
```
* HubName The HubName property specifies DurableTaskHubName. Please set same as TaskHub specified by Durable functions.
* OffsetHour You can specify the Offset time from Utc of the time displayed on the screen.
