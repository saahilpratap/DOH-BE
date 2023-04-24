using LockthreatCompliance.Dto;
using LockthreatCompliance.FeedBacks.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FeedBacks.Exporting
{
  public  interface IFeedBackExcelExporter
    {
        FileDto ExportToFile(List<FeedBackExcelDto> feedbackresponse );
    }
}
