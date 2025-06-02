import React, { createContext, useContext } from "react";
import SensorMinDto from "../interfaces/sensor_min_dto";
import useSWR from "swr";
import { getSensorsFetchEndPoint, getSensorsFetch } from "../api/get_sensots_fetch";

interface SensorsContextType {
    sensors: SensorMinDto[],
    sensorsMutate: () => void
}
const SensorsContext = createContext<SensorsContextType | undefined>(undefined)

const SensorsProvider: React.FC<{children: React.ReactNode}> = ({children}) =>{
  
    const {data:sensorsData, mutate} = useSWR(getSensorsFetchEndPoint,getSensorsFetch)
    const sensors = sensorsData ?? []
    const sensorsMutate = () => mutate()
    return(
        <SensorsContext.Provider value={{sensors,sensorsMutate}}>
            {children}
        </SensorsContext.Provider>
    )
}
const useSensors = ():SensorsContextType =>{
    const context = useContext(SensorsContext)
    if(!context)
        throw new Error('Sensors context not found')
    return context
}
export { SensorsProvider, useSensors}