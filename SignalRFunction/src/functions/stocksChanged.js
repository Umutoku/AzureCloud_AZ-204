const { app } = require('@azure/functions');

app.cosmosDB('stocksChanged', {
    connectionStringSetting: 'AzureWebJobsStorage',
    databaseName: 'stocksdb',
    collectionName: 'stocks',
    createLeaseCollectionIfNotExists: true,
    feedPollDelay: 500,
    handler: (documents, context) => {
        context.log(`Cosmos DB function processed ${documents.length} documents`);
    }
});

module.exports = async function (context, documents) {
    const updates = documents.map(stock => ({
        target: 'updated',
        arguments: [stock]
    }));

    context.bindings.signalRMessages = updates;
    context.done();
};

// function.json
// {
//     "type": "cosmosDBTrigger",
//     "name": "documents",
//     "direction": "in",
//     "leaseCollectionName": "leases",
//     "connectionStringSetting": "AzureCosmosDBConnectionString",
//     "databaseName": "stocksdb",
//     "collectionName": "stocks",
//     "createLeaseCollectionIfNotExists": "true",
//     "feedPollDelay": 500
//   }