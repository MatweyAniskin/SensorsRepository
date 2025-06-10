import { Popup } from "react-leaflet"
import SensorMinDto from "../interfaces/sensor_min_dto"

const MapSensorPopup = ({
   sensor
}: {
   sensor:SensorMinDto
}) => {
   
    return (
        <>
            <Popup>
                <p>{`Название сенсора: ${sensor.sensor_name}`}</p>
                <p>{`Последние измерения`}</p>
                <p>{`Данные 1: ${sensor.value1}, Данные 2: ${sensor.value2}`}</p>
                <p>{`Всего измерений: ${sensor.value_count}`}</p>
            </Popup>
        </>
    )
}
export default MapSensorPopup