using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Skyscraper
{
    public class Broadcaster
    {
        private readonly IHubContext _hubContext;
        private BasicShapeModel _model;
        public Broadcaster(IConnectionManager connectionManager)
        {
            // Save our hub context so we can easily use it 
            // to send to its connected clients
            _hubContext = connectionManager.GetHubContext<SkyscraperHub>();
            _model = new BasicShapeModel();
        }
        public void UpdateShape(BasicShapeModel clientModel)
        {
            _model = clientModel;
            _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
        }
    }
}