using core.Enum;
using core.Util;
using core.Common.Request.Transfer;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;

namespace core.Common.Feature
{
    internal sealed class Transfer : Feature
    {
        public TransferCreation InitialRequestData { get; set; }
        public TransferBaseRequest RequestData { get; set; }
        public string RequestNumber { get; set; }

        public Transfer Initialize(TransferCreation requestData)
        {
            this.InitialRequestData = requestData;
            Logger.Log.Info("ID taken for test is: " + requestData.ItemsToTransfer[0].ToString());
            return this;
        }

        public string GetExpectedPayloadByAction(FlowActions actionName)
        {
            switch (actionName)
            {
                case FlowActions.CREATE:
                    return FileHelper.ReadJsonStringFromFile("some file");

                case FlowActions.ACCEPT:
                    return FileHelper.ReadJsonStringFromFile("some file");

                case FlowActions.REJECT:
                    return FileHelper.ReadJsonStringFromFile("some file");

                case FlowActions.CANCEL:
                    return FileHelper.ReadJsonStringFromFile("some file");
                default:
                    throw new NoTestDataFoundException("No Payload found for the provided transfer action");
            }
        }

        public Transfer Create()
        {
            var json = JsonConvert.SerializeObject(InitialRequestData);

            Logger.Log.Info("POST request to create a transfer for equipment item " +
                InitialRequestData.ItemsToTransfer[0]);

            RestResponse = RestClient.ExecutePOSTRequestAsUser(
                EndPointHelper.TRANSFER_PATH,
                new Dictionary<string, string> { { "login", "password" } }, json);
            Logger.Log.Info("Response upon transfer creation is: " + RestResponse.Content);

            return this;
        }

        public Transfer SetRequestNumber()
        {
            var response = JsonConvert.DeserializeObject<TransferAction>(RestResponse.Content);
            RequestNumber = response.Object.Substring(response.Object.LastIndexOf(' ') + 1);

            Logger.Log.Info("Request Number is " + RequestNumber);
            return this;
        }

        public Transfer ValidateResponse(FlowActions actionName)
        {
            RestResponse.StatusCode.ToString().Should().Be("200");

            string expectedPayload = GetExpectedPayloadByAction(actionName);
            expectedPayload = expectedPayload.Replace("PLACEHOLDER", RequestNumber);

            AssertionHelper.AssertResponsesByPayLoad<TransferAction>(expectedPayload, RestResponse.Content);

            return this;
        }

        public Transfer Accept()
        {
            Logger.Log.Info("Get received transfers");
            var json = RestClient.ExecuteGETRequestAsUser(
                EndPointHelper.TRANSFER_PATH,
                new Dictionary<string, string> { { "login", "password" } }).Content;

            if (json.Equals(string.Empty))
                throw new NoTestDataFoundException("No transfers received for " + "UserName");

            Logger.Log.Info("Find transfer number: " + RequestNumber);
           
            List<TransferBaseRequest> payload = new List<TransferBaseRequest>() { RequestData };

            json = JsonConvert.SerializeObject(payload);

            Logger.Log.Info("Execute POST to approve transfer");
            RestResponse = RestClient.ExecutePOSTRequestAsUser(
                EndPointHelper.TRANSFER_PATH,
                new Dictionary<string, string> { { "login", "password" } }, json);

            return this;
        }
    }
}