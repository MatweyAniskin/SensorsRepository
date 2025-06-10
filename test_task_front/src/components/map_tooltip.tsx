import { Tooltip } from "react-leaflet"

const MapSensorToolTip = ({
    sensorName
}: {
    sensorName: string
}) => {
    return (
        <>
            <Tooltip>
                {sensorName}
            </Tooltip>
        </>
    )
}
export default MapSensorToolTip