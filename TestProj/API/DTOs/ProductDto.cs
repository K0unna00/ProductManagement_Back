﻿using MongoDB.Bson.Serialization.Attributes;

namespace TestProj.API.DTOs;

public class ProductDto
{

    public string? Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public IFormFile Image { get; set; }

    public string? ImgBase64 { get; set; }
}
