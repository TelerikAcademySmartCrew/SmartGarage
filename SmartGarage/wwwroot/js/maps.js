// Bing Maps Lib

//<script async defer src="https://www.bing.com/api/maps/mapcontrol?branch=release&callback=loadMapScenario" async defer></script>
//<script async defer src="https://www.bing.com/api/maps/mapcontrol?callback=GetMap&key=ApvRORj156hj-Yz4V_afezO6aKGaah_wlNO_WSnL7sRJ6igeOx_6LFB2biXEnFGu" async defer></script>

var map;
var infobox;
var latitude = 42.65033853376936;
var longitude = 23.379256507391496;
var title = 'SmartGarage';
var description = 'SmartGarage';

// Bing Maps Lib
function loadBingMapsScript(callback) {
    return new Promise((resolve, reject) => {
        var bingMapsScript = document.createElement('script');
        bingMapsScript.type = 'text/javascript';

        bingMapsScript.src = 'https://www.bing.com/api/maps/mapcontrol?branch=release&callback=loadMapScenario';
        bingMapsScript.async = true;
        bingMapsScript.defer = true;

        bingMapsScript.onload = () => {
            loadMapScenario();
            resolve();
        };

        bingMapsScript.onerror = () => {
            reject(new Error('Failed to load Bing Maps script.'));
        };

        document.head.appendChild(bingMapsScript);
    });
}

// Bing Maps Key
function loadMapScenario() {
    var bingMapsKeyScript = document.createElement('script');
    bingMapsKeyScript.type = 'text/javascript';
    bingMapsKeyScript.src = 'https://www.bing.com/api/maps/mapcontrol?callback=GetMap&key=ApvRORj156hj-Yz4V_afezO6aKGaah_wlNO_WSnL7sRJ6igeOx_6LFB2biXEnFGu';
    bingMapsKeyScript.async = true;
    bingMapsKeyScript.defer = true;
    document.head.appendChild(bingMapsKeyScript);
}

function showInfobox(e) {
    if (e.target.metadata) {
        infobox.setOptions({
            location: e.target.getLocation(),
            title: e.target.metadata.title,
            description: e.target.metadata.description,
            visible: true
        });
    }
}

function hideInfobox(e) {
    infobox.setOptions({ visible: false });
}

// Declare addMarker function
function AddMarker(latitude, longitude, title, description) {
    var location = new Microsoft.Maps.Location(latitude, longitude);
    var marker = new Microsoft.Maps.Pushpin(location, { color: 'green' });

    infobox = new Microsoft.Maps.Infobox(marker.getLocation(), {
        visible: false
    });

    marker.metadata = {
        title: title,
        description: description
    };

    Microsoft.Maps.Events.addHandler(marker, 'mouseout', hideInfobox);
    Microsoft.Maps.Events.addHandler(marker, 'mouseover', showInfobox);
    
    infobox.setMap(map);
    map.entities.push(marker);
    marker.setOptions({ enableHoverStyle: true });

    // Focus the map on the added marker
    map.setView({ center: location, zoom: 10 });
}

function GetMap() {
    document.cookie = "myCookie=myValue; Secure; SameSite=None";
    
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: 'ApvRORj156hj-Yz4V_afezO6aKGaah_wlNO_WSnL7sRJ6igeOx_6LFB2biXEnFGu'
    });
    AddMarker(latitude, longitude, title, description);
}

async function LoadMap() {
    await loadBingMapsScript();
}
