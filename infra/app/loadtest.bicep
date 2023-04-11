param name string
param location string = resourceGroup().location
param tags object = {}

resource loadtest 'Microsoft.LoadTestService/loadTests@2022-12-01' = {
  name: name
  location: location
  tags: tags
  identity: {
    type: 'None'
  }
  properties: {
  }
}

output AZURE_LOAD_TEST_NAME string = loadtest.name
