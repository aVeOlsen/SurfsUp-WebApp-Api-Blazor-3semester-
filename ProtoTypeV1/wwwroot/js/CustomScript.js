$(function () {
	$(document).scroll(function () {
		var $nav = $(".navbar");
		$nav.toggleClass('scrolled', $(this).scrollTop() > $nav.height());
		$nav.toggleClass('shadow', $(this).scrollTop() > $nav.height());
	});
});

$(".jumbotron").css({ height: $(window).height() + "px" });

$(window).on("resize", function () {
	$(".jumbotron").css({ height: $(window).height() + "px" });
});

function MapUpdate() {

	if (document.getElementById("SearchLocation").value) {

		var SearchLocationName = document.getElementById("SearchLocation").value;
		var Latitude = "0";
		var Longitude = "0";
		var Zoom = "14";


		$.ajax({
			url: " https://maps.googleapis.com/maps/api/geocode/json?address=" + SearchLocationName + ",+CA&key=AIzaSyByUufDE9K8yiQDMu1YqyzL4RYpe_-MCNs",
			type: "GET",
			success: function (result) {
				alert("success");
				console.log(result);

				Latitude = result.results[0].geometry.location.lat;
				Longitude = result.results[0].geometry.location.lng;



				document.getElementById("WindMap").src = "https://embed.windy.com/embed2.html?lat=" + Latitude + "&lon=" + Longitude + "&detailLat=" + Latitude + "&detailLon=" + Longitude + "&width=650&height=450&zoom=" + Zoom + "&level=surface&overlay=waves&product=ecmwfWaves&menu=&message=&marker=&calendar=now&pressure=&type=map&location=coordinates&detail=&metricWind=default&metricTemp=default&radarRange=-1";
				document.getElementById("WaveMap").src = "https://embed.windy.com/embed2.html?lat=" + Latitude + "&lon=" + Longitude + "&detailLat=" + Latitude +  "&detailLon=" + Longitude + "&width=650&height=450&zoom=" + zoom + "&level=surface&overlay=wind&product=ecmwf&menu=&message=&marker=&calendar=now&pressure=&type=map&location=coordinates&detail=&metricWind=km%2Fh&metricTemp=%C2%B0C&radarRange=-1";

				document.getElementById("Latitude").innerHTML = Latitude;
				document.getElementsById("Longitude").innerHTML = Longitude;
			},
			error: function (error) {
				alert("error");
				console.log(error);
			}
		}
		)
	}

}

