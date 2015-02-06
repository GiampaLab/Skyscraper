using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Skyscraper
{
    public class SkyscraperHub : Hub
    {
        // Is set via the constructor on each creation
        private readonly IHubContext _hubContext;
        private BasicShapeModel _model;
        /*public SkyscraperHub()
            : this(Broadcaster.Instance)
        {
        }*/
        public SkyscraperHub(IConnectionManager connectionManager)
        {
            _hubContext = connectionManager.GetHubContext<SkyscraperHub>();
            _model = new BasicShapeModel();
        }
        public void UpdateModel(BasicShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            _model = clientModel;
            _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
        }

        public void AddBox()
        {
            //mouseConstraint.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(Context.ConnectionId).updateBox();
        }
        public void AddCircle()
        {
            //mouseConstraint.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(Context.ConnectionId).updateCircle();
        }
    }
}