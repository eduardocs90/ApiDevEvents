﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwersomeDevEvents.API.Entities
{
    public class DevEventSpeaker
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TalkTitle { get; set; }
        public string TalkDescription { get; set; }

        public string LinkedinProfile { get; set; }

        public Guid DevEventId { get; set; }

    }
}