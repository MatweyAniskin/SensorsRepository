import { Popup } from "react-leaflet"

const MapSensorPopup = ({
    sensorName,
    sensorValues
}: {
    sensorName:string
    sensorValues:number[]
}) => {
    const sensorsValuesMin = sensorValues.reverse().slice(0,3)
    return (
        <>
            <Popup>
                <p>{`Название сенсора: ${sensorName}`}</p>
                {
                    sensorsValuesMin.map((i,k) => (
                        <p key={k}>{`Данные ${k+1}: ${i}`}</p>
                    ))
                }
            </Popup>
        </>
    )
}
export default MapSensorPopup