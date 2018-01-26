using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenHost.CORS
{
    public class GenCors
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "Host", "CORS");
        string _Namespace = Config.GetMainNamespace();
        StringBuilder _contentsBehavior = new StringBuilder();
        StringBuilder _contentsMessage = new StringBuilder();

        public GenCors()
        {

        }

        public void CreateCors()
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/BehaviorAttribute.cs").Close();
                File.Create(_filePath + "/MessageInspector.cs").Close();

                CreateBehaviorAttribute();
                CreateBehaviorAttribute();

                Save();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateBehaviorAttribute()
        {
            #region usings
            _contentsBehavior.AppendLine("using System;");
            _contentsBehavior.AppendLine("using System.Collections.Generic;");
            _contentsBehavior.AppendLine("using System.Linq;");
            _contentsBehavior.AppendLine("using System.ServiceModel.Channels;");
            _contentsBehavior.AppendLine("using System.ServiceModel.Description;");
            _contentsBehavior.AppendLine("using System.ServiceModel.Dispatcher;");
            _contentsBehavior.AppendLine("using System.Text;");
            _contentsBehavior.AppendLine("using System.Threading.Tasks;");
            #endregion usings

            #region Namespace
            _contentsBehavior.AppendLine(@"namespace drBonusingServiceHost.CORS
{ ");
            #region Class
            _contentsBehavior.AppendLine(@"public class BehaviorAttribute : Attribute, IEndpointBehavior,
                                 IOperationBehavior
    {");
            // content of class
            _contentsBehavior.AppendLine(@"public void Validate(ServiceEndpoint endpoint) { }

        public void AddBindingParameters(ServiceEndpoint endpoint,
                                 BindingParameterCollection bindingParameters) { }

        /// <summary>
        /// This service modify or extend the service across an endpoint.
        /// </summary>
        /// <param name=\""endpoint\"">The endpoint that exposes the contract.</param>
        /// <param name=\""endpointDispatcher\"">The endpoint dispatcher to be
        /// modified or extended.</param>
            public void ApplyDispatchBehavior(ServiceEndpoint endpoint,
                                          EndpointDispatcher endpointDispatcher)
            {
                // add inspector which detects cross origin requests
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                                                       new MessageInspector());
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint,
                                            ClientRuntime clientRuntime)
            { }

            public void Validate(OperationDescription operationDescription) { }

            public void ApplyDispatchBehavior(OperationDescription operationDescription,
                                              DispatchOperation dispatchOperation)
            { }

            public void ApplyClientBehavior(OperationDescription operationDescription,
                                            ClientOperation clientOperation)
            { }

            public void AddBindingParameters(OperationDescription operationDescription,
                                      BindingParameterCollection bindingParameters)
            { }
            ");
            _contentsBehavior.AppendLine("}");
            #endregion Class
            _contentsBehavior.AppendLine("}");
            #endregion Namespace
        }

        private void CreateMessageInspector()
        {
            #region usings
            _contentsMessage.AppendLine("using System;");
            _contentsMessage.AppendLine("using System.Collections.Generic;");
            _contentsMessage.AppendLine("using System.Linq;");
            _contentsMessage.AppendLine("using System.ServiceModel.Channels;");
            _contentsMessage.AppendLine("using System.ServiceModel.Description;");
            _contentsMessage.AppendLine("using System.ServiceModel.Dispatcher;");
            _contentsMessage.AppendLine("using System.Text;");
            _contentsMessage.AppendLine("using System.Threading.Tasks;");
            #endregion usings

            #region Namespace
            _contentsMessage.AppendLine(@"namespace drBonusingServiceHost.CORS
{ ");
            #region Class 1
            _contentsMessage.AppendLine(@"public class MessageInspector : IDispatchMessageInspector
    {
    {");
            // content of class
            _contentsMessage.AppendLine(@"/// <summary>
        /// Called when an inbound message been received
        /// </summary>
        /// <param name=\""request\"">The request message.</param>
        /// <param name=\""channel\"" > The incoming channel.</param>
        /// <param name=\""instanceContext\"" > The current service instance.</param>
        /// <returns>
        /// The object used to correlate stateMsg. 
        /// This object is passed back in the method.
        /// </returns>
            public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
            {
                StateMessage stateMsg = null;
                HttpRequestMessageProperty requestProperty = null;
                if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    requestProperty = request.Properties[HttpRequestMessageProperty.Name]
                                      as HttpRequestMessageProperty;
                }

                if (requestProperty != null)
                {
                    var origin = requestProperty.Headers[\""Origin\""];
                    if (!string.IsNullOrEmpty(origin))
                    {
                        stateMsg = new StateMessage();
                        // if a cors options request (preflight) is detected, 
                        // we create our own reply message and don't invoke any 
                        // operation at all.
                        if (requestProperty.Method == \""OPTIONS\"")
                        {
                            stateMsg.Message = Message.CreateMessage(request.Version, null);
                        }
                        request.Properties.Add(\""CrossOriginResourceSharingState\"", stateMsg);
                    }
                }

                return stateMsg;
            }

            public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
            {
                var stateMsg = correlationState as StateMessage;

                if (stateMsg != null)
                {
                    if (stateMsg.Message != null)
                    {
                        reply = stateMsg.Message;
                    }

                    HttpResponseMessageProperty responseProperty = null;

                    if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                    {
                        responseProperty = reply.Properties[HttpResponseMessageProperty.Name]
                                           as HttpResponseMessageProperty;
                    }

                    if (responseProperty == null)
                    {
                        responseProperty = new HttpResponseMessageProperty();
                        reply.Properties.Add(HttpResponseMessageProperty.Name,
                                             responseProperty);
                    }

                    // Access-Control-Allow-Origin should be added for all cors responses
                    responseProperty.Headers.Set(\""Access -Control-Allow-Origin\"", \"" * \"");

                    if (stateMsg.Message != null)
                    {
                        // the following headers should only be added for OPTIONS requests
                        responseProperty.Headers.Set(\""Access -Control-Allow-Methods\"",
                                                     \""POST, OPTIONS, GET\"");
                        responseProperty.Headers.Set(\""Access -Control-Allow-Headers\"",
                                  \""Content -Type, Accept, Authorization, x-requested-with\"");
                    }
                }
            }
            ");
            _contentsMessage.AppendLine("}");
            #endregion Class

            #region Class 2
            _contentsMessage.AppendLine(@"class StateMessage
    {
        public Message Message;
    }");
            #endregion Class 2
            _contentsMessage.AppendLine("}");
            #endregion Namespace
        }

        private void Save()
        {
            try
            {
                File.WriteAllText(_filePath + "/BehaviorAttribute.cs", _contentsBehavior.ToString());
                Console.WriteLine("Added new service cors file : BehaviorAttribute.cs");

                File.WriteAllText(_filePath + "/MessageInspector.cs", _contentsMessage.ToString());
                Console.WriteLine("Added new service cors file : MessageInspector.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
