using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Skyscraper
{
    public class SkyscraperHub : Hub
    {
        // Is set via the constructor on each creation
	    private Broadcaster _broadcaster;
	    public SkyscraperHub()
	        : this(Broadcaster.Instance)
	    {
	    }
        public SkyscraperHub(Broadcaster broadcaster)
	    {
	        _broadcaster = broadcaster;
	    }
        public void UpdateModel(BasicShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            _broadcaster.UpdateShape(clientModel);
        }

        public void addBox()
        {
            //mouseConstraint.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(Context.ConnectionId).updateBox();
        }
        public void addCircle()
        {
            //mouseConstraint.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(Context.ConnectionId).updateCircle();
        }
    }
}