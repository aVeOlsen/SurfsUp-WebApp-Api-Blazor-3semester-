function UpdateSpotMap(){




    var lat = "";
    var lng =  "";

	if(document.getElementById("ChangeMapBtn").value == "Vindkort"){
		
	}else{

	}
}


window.onload = GetGeoReverseLocation(); 


function GetGeoReverseLocation(){

    var SearchLocationName = document.getElementById("LocationName").innerHTML;
    console.log(SearchLocationName)
    var SearchLocationName = SearchLocationName.replace("Address: ", "Denmark ");

    $.ajax({
        url: " https://maps.googleapis.com/maps/api/geocode/json?address=" + SearchLocationName + ",+CA&key=AIzaSyByUufDE9K8yiQDMu1YqyzL4RYpe_-MCNs",
        type: "GET",
        success: function (result) {
            if((document.getElementById("lat").value = true) && (document.getElementById("lng").value = true)){
                alert("success");
                document.getElementById("lat").innerHTML = "<strong>Latitude: </strong> " + result.results[0].geometry.location.lat;
                document.getElementById("lng").innerHTML = "<strong>Longitude: </strong>" + result.results[0].geometry.location.lng;
                document.getElementById("spotMap").src = "https://embed.windy.com/embed2.html?lat=" + result.results[0].geometry.location.lat + "&lon=" + result.results[0].geometry.location.lng + "&detailLat=" + result.results[0].geometry.location.lat + "&detailLon="+ result.results[0].geometry.location.lng + "&width=650&height=450&zoom=15&level=surface&overlay=wind&product=ecmwf&menu=&message=&marker=true&calendar=now&pressure=true&type=map&location=coordinates&detail=true&metricWind=m%2Fs&metricTemp=%C2%B0C&radarRange=-1";

            }else{
                document.getElementById("lat").innerHTML = "<strong>Latitude: </strong> " + result.results[0].geometry.location.lat;
                document.getElementById("lng").innerHTML = "<strong>Longitude: </strong>" + result.results[0].geometry.location.lng;
            }
            
        },
        error: function (error) {
            alert("error");
            console.log(error);
        }
    })

}

var changeMap = document.getElementById("changeMap");

changeMap.onclick = function(){
    if(document.getElementById("ChangeMapBtn").innerHTML = "VindKort"){
        document.getElementById("ChangeMapBtn").innerHTML = "BølgeKort";
        //document.getElementById("spotMap").src = "https://embed.windy.com/embed2.html?lat=" + result.results[0].geometry.location.lat + "&lon=" + result.results[0].geometry.location.lng + "&detailLat=" + result.results[0].geometry.location.lat + "&detailLon="+ result.results[0].geometry.location.lng + "&width=650&height=450&zoom=15&level=surface&overlay=wind&product=ecmwf&menu=&message=&marker=true&calendar=now&pressure=true&type=map&location=coordinates&detail=true&metricWind=m%2Fs&metricTemp=%C2%B0C&radarRange=-1";

    }else{
        document.getElementById("ChangeMapBtn").innerHTML = "Vindkort";
        //document.getElementById("spotMap").src = "https://embed.windy.com/embed2.html?lat=" + result.results[0].geometry.location.lat + "&lon=" + result.results[0].geometry.location.lng + "&detailLat=" + result.results[0].geometry.location.lat + "&detailLon="+ result.results[0].geometry.location.lng + "&width=650&height=450&zoom=15&level=surface&overlay=wave&product=ecmwf&menu=&message=&marker=true&calendar=now&pressure=true&type=map&location=coordinates&detail=true&metricWind=m%2Fs&metricTemp=%C2%B0C&radarRange=-1";
    }
}


