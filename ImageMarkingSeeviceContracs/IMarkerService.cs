using ImakeMarkingDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageMarkingServiceContracs
{
    public interface IMarkerService
    {
        public CreateMarkerResponseDTO CreateMarker(MarkerDTO marker);
        public BoolResponseDTO RemoveMarker(string markerID);

        public BoolResponseDTO UpdateMarker(MarkerDTO marker);

        public GetMarkersResponseDTO GetMarkers(string documentID);
    }
}
