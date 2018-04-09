using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;

namespace MediaPlayerRuntimeComponent
{
    public sealed class MediaPlayerVoiceCommandService : IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDeferral;
        VoiceCommandServiceConnection voiceServiceConnection;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {

            //Service deferral to prevent service termination
            this.serviceDeferral = taskInstance.GetDeferral();
            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails != null && triggerDetails.Name == "MediaPlayerVoiceCommandService")
            {
                try
                {
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceServiceConnection.VoiceCommandCompleted += VoiceCommandCompleted;

                    /* Note: these background commands don't do anything interesting and are simply here to demonstrate
                     * that commands can be run in the background using Cortana as part of this project. It is possible
                     * to add commands relevant to this application such as playing a song in the background however
                     * it's not practical since Cortana must provide progress every 5 seconds and besides the user can
                     * give a command to play a song and simply minimize the app and the song continues to play while
                     * the app is in the suspended state.
                     */
                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
                    switch (voiceCommand.CommandName)
                    {
                        case "backgroundTask":
                            {
                                var userMessage = new VoiceCommandUserMessage();
                                userMessage.DisplayMessage = "Processing your request";
                                userMessage.SpokenMessage = "Processing your request";
                                var progressReport = VoiceCommandResponse.CreateResponse(userMessage);
                                await voiceServiceConnection.ReportProgressAsync(progressReport);
                                try
                                {
                                    userMessage = new VoiceCommandUserMessage();
                                    userMessage.DisplayMessage = "Executing your request";
                                    userMessage.SpokenMessage = "Executing your request";

                                    await voiceServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                                }
                                catch (Exception)
                                {
                                    userMessage.DisplayMessage = "Something went wrong, terminating process";
                                    userMessage.SpokenMessage = "Something went wrong, terminating process";
                                    await voiceServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                                }
                                break;
                            }
                        case "backgroundTask2":
                            {
                                var userMessage = new VoiceCommandUserMessage();
                                userMessage.DisplayMessage = "Processing your request";
                                userMessage.SpokenMessage = "Processing your request";
                                var progressReport = VoiceCommandResponse.CreateResponse(userMessage);
                                await voiceServiceConnection.ReportProgressAsync(progressReport);
                                try
                                {
                                    userMessage = new VoiceCommandUserMessage();
                                    userMessage.DisplayMessage = "Executing your request";
                                    userMessage.SpokenMessage = "Executing your request";

                                    await voiceServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                                }
                                catch (Exception)
                                {
                                    userMessage.DisplayMessage = "Something went wrong, terminating process";
                                    userMessage.SpokenMessage = "Something went wrong, terminating process";
                                    await voiceServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                                }
                                break;
                            }
                    }
                }
                finally
                {
                    if (this.serviceDeferral != null)
                    {
                        //Complete the service deferral
                        this.serviceDeferral.Complete();
                    }
                }
            }
        }
        //Service completed
        private void VoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            if (this.serviceDeferral != null)
            {
                this.serviceDeferral.Complete();
            }
        }
    }
}