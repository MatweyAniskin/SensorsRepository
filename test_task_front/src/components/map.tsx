import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import { Icon } from 'leaflet';
import SensorMinDto from '../interfaces/sensor_min_dto';
import MapSensorPopup from './map_popup';
const flagIcon = new Icon({
    iconUrl: '/flag.png', 
    iconSize: [35, 35], 
    iconAnchor: [12, 41], 
    popupAnchor: [1, -34], 
});
const SensorsMap = ({
    sensors
}: {
    sensors: SensorMinDto[]
}) => {
    return (
        <MapContainer center={[0,0]} zoom={4} style={{ height: "100vh", width: "100%" }}>
            <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                 url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
             
            />
            {
                sensors.map((i, k) => (
                    <Marker key={k} position={[i.north, i.east]} icon={flagIcon}>
                       <MapSensorPopup sensorName={i.sensor_name} sensorValues={i.values}/>
                    </Marker>
                ))
            }

        </MapContainer>
    );
}
export default SensorsMap