import SensorsMap from "../components/map"
import { useSensors } from "../components/sensors_provider"

const MapPage = () => {
    const {sensors} = useSensors()
    return (
        <>
            <SensorsMap sensors={sensors} />
        </>
    )
}
export default MapPage