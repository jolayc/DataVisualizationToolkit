//-----------------------------------------------------------------------
// <copyright file="MapMatchingParameters.cs" company="Mapbox">
//     Copyright (c) 2017 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel;
using DescriptionAttr = System.ComponentModel.DescriptionAttribute;
using Mapbox.VectorTile.Geometry;

namespace Mapbox.MapMatching
{
	/// <summary>Directions profile id</summary>
	public enum Profile
	{
		[DescriptionAttr("mapbox/driving")]
		MapboxDriving,
		[DescriptionAttr("mapbox/driving-traffic")]
		MapboxDrivingTraffic,
		[DescriptionAttr("mapbox/walking")]
		MapboxWalking,
		[DescriptionAttr("mapbox/cycling")]
		MapboxCycling
	}


	/// <summary>Format of the returned geometry. Default value 'Polyline' with precision 5.</summary>
	public enum Geometries
	{
		/// <summary>Default, precision 5.</summary>
		[DescriptionAttr("polyline")]
		Polyline,
		/// <summary>Precision 6.</summary>
		[DescriptionAttr("polyline6")]
		Polyline6,
		/// <summary>Geojson.</summary>
		[DescriptionAttr("geojson")]
		GeoJson
	}


	/// <summary>Type of returned overview geometry. </summary>
	public enum Overview
	{
		/// <summary>The most detailed geometry available </summary>
		[DescriptionAttr("full")]
		Full,
		/// <summary>A simplified version of the full geometry</summary>
		[DescriptionAttr("simplified")]
		Simplified,
		/// <summary>No overview geometry </summary>
		[DescriptionAttr("false")]
		None
	}


	/// <summary>Whether or not to return additional metadata along the route. Several annotations can be used.</summary>
	[System.Flags]
	public enum Annotations
	{
		[DescriptionAttr("duration")]
		Duration,
		[DescriptionAttr("distance")]
		Distance,
		[DescriptionAttr("speed")]
		Speed,
		[DescriptionAttr("congestion")]
		Congestion
	}


	/// <summary>
	/// https://www.mapbox.com/api-documentation/navigation/#retrieve-directions
	/// </summary>
	public enum InstructionLanguages
	{
		[DescriptionAttr("de")]
		German,
		[DescriptionAttr("en")]
		English,
		[DescriptionAttr("eo")]
		Esperanto,
		[DescriptionAttr("es")]
		Spanish,
		[DescriptionAttr("es-ES")]
		SpanishSpain,
		[DescriptionAttr("fr")]
		French,
		[DescriptionAttr("id")]
		Indonesian,
		[DescriptionAttr("it")]
		Italian,
		[DescriptionAttr("nl")]
		Dutch,
		[DescriptionAttr("pl")]
		Polish,
		[DescriptionAttr("pt-BR")]
		PortugueseBrazil,
		[DescriptionAttr("ro")]
		Romanian,
		[DescriptionAttr("ru")]
		Russian,
		[DescriptionAttr("sv")]
		Swedish,
		[DescriptionAttr("tr")]
		Turkish,
		[DescriptionAttr("uk")]
		Ukrainian,
		[DescriptionAttr("vi")]
		Vietnamese,
		[DescriptionAttr("zh-Hans")]
		ChineseSimplified
	}


}
