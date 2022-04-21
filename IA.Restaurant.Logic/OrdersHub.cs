using Microsoft.AspNetCore.SignalR;

namespace IA.Restaurant.Logic
{
    public class OrdersHub : Hub
    {
        public static int conectados { get; set; }
        /// <summary>
        /// send message when connecting WS
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Conexión exitosa", Context?.User?.Identity?.Name, "joined");
        }
        /// <summary>
        /// send message when disconnecting WS
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Caller.SendAsync("Desconexión exitosa", Context?.User?.Identity?.Name, "left");
        }
    }
}
