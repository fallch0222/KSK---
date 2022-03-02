using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        DiscordSocketClient client; //봇 클라이언트
        CommandService commands;    //명령어 수신 클라이언트
        /// <summary>
        /// 프로그램의 진입점
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            new Program().BotMain().GetAwaiter().GetResult();   //봇의 진입점 실행
        }

        /// <summary>
        /// 봇의 진입점, 봇의 거의 모든 작업이 비동기로 작동되기 때문에 비동기 함수로 생성해야 함
        /// </summary>
        /// <returns></returns>
        public async Task BotMain()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig() {    //디스코드 봇 초기화
                LogLevel = LogSeverity.Verbose                              //봇의 로그 레벨 설정 
            });
            commands = new CommandService(new CommandServiceConfig()        //명령어 수신 클라이언트 초기화
            {
                LogLevel = LogSeverity.Verbose                              //봇의 로그 레벨 설정
            });

            //로그 수신 시 로그 출력 함수에서 출력되도록 설정
            client.Log += OnClientLogReceived;    
            commands.Log += OnClientLogReceived;

            await client.LoginAsync(TokenType.Bot, "token"); //봇의 토큰을 사용해 서버에 로그인
            await client.StartAsync();                         //봇이 이벤트를 수신하기 시작

            client.MessageReceived += OnClientMessage;         //봇이 메시지를 수신할 때 처리하도록 설정
            
           
           

            

            await Task.Delay(-1);   //봇이 종료되지 않도록 블로킹
        }

        public bool WillItMention = true;
        private async Task OnClientMessage(SocketMessage arg)
        {
            //수신한 메시지가 사용자가 보낸 게 아닐 때 취소
            var message = arg as SocketUserMessage;
            if (message == null) return;

            //메시지 앞에 !이 달려있지 않고, 자신이 호출된게 아니거나 다른 봇이 호출했다면 취소
            if (message.Author.IsBot)
                return;

            ulong channelID = Convert.ToUInt64(945660888336187442); 
            var textChannel = (SocketTextChannel)client.GetChannel(channelID);
            if (message.Author.Id == 371571375116386304)
            {
                if (message.Content.Substring(0, 1) == "!" && message.Content.Substring(1,4) == "긴급삭제")
                {
                    UInt64 willDel = Convert.ToUInt64(message.Content.Split(' ')[1]);
                    

                  

                    await textChannel.DeleteMessageAsync(willDel);

                    EmbedBuilder embedBuilder = new EmbedBuilder();
                    embedBuilder.WithTitle("삭제 완료");
                    embedBuilder.WithColor(Color.Red);
                    message.Channel.SendMessageAsync(embed: embedBuilder.Build());


                }
            }
            
            if (message.Channel.Id == 877042810551947324)
            {
                if (message.Content.Substring(0, 1) == "!" && message.Content.Substring(1,4) == "긴급삭제")
                {
                    UInt64 willDel = Convert.ToUInt64(message.Content.Split(' ')[1]);
                    

                  

                    await textChannel.DeleteMessageAsync(willDel);

                    EmbedBuilder embedBuilder = new EmbedBuilder();
                    embedBuilder.WithTitle("삭제 완료");
                    embedBuilder.WithColor(Color.Red);
                    message.Channel.SendMessageAsync(embed: embedBuilder.Build());


                }
            }
            Task.Delay(10);
            if (!message.Channel.Name.Contains("질문방"))
                return;
            
             
            
            var context = new SocketCommandContext(client, message);                    //수신된 메시지에 대한 컨텍스트 생성

           

            EmbedBuilder eb = new EmbedBuilder();

            string fieldContent = message.Author.Mention + $@"님이 <#" + context.Channel.Id + ">에 **[" + message.Content +
                                  $"]({message.GetJumpUrl()})**에 대한 질문을 개시하셨습니다!";

            string mentionContents = "none";
            if (WillItMention)
            {
                mentionContents = mentionMessageContent(message.Channel.Id);

                if (mentionContents != "none")
                {
                    WillItMention = false;
                    mentionTimer(600000);
                }
                

            }
            
            eb.AddField("질문 알림!", fieldContent);

            if (mentionContents != "none")
            {
                textChannel.SendMessageAsync(embed: eb.Build(), text:"\n||" + mentionContents + "||"); 
            }

            else
            {
                textChannel.SendMessageAsync(embed: eb.Build()); 
            }
        }

        public string mentionMessageContent(UInt64 ID)
        {
            switch (ID)
            {
                case 779641423502770206: //python
                    return "<@&780316135282704395>";
                
                case  780296209939431424: //java
                    return "<@&780316403814498324>";
                
                case 779978769493655563: //c and cpp
                    return "<@&780281390897168416>";
                
                case 817250922211573791: //Html and Css
                    return "<@&819377019342815272>";
                
                case 779978564723277845: //JavaScript
                    return "<@&780316494906261525>";
                
                case 801735664528785418: //TypeScript
                    return "<@&948132537371340830>";
                
                case 787629853692264468: //Csharp
                    return "<@&877406923899674624>";
                
                case 921627644016066640: //Unity
                    return "<@&948123651713146890>";
                
                case 932564429277626398: //linux
                    return "<@&948126355365720065>";
                
                case 821212431568207913: //others
                    return "none";
                
                default:
                    return "ERROR!!! CH. ID NOT FOUND";
            }
        }

        public async Task mentionTimer(int MS)
        {
            await Task.Delay(MS);
            WillItMention = true;
            return;
        }
        /// <summary>
        /// 봇의 로그를 출력하는 함수
        /// </summary>
        /// <param name="msg">봇의 클라이언트에서 수신된 로그</param>
        /// <returns></returns>
        private Task OnClientLogReceived(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());  //로그 출력
            return Task.CompletedTask;
        }
    }
}
