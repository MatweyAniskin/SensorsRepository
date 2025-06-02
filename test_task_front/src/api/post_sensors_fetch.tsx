export const postSensorsFetch = async (file:any) => {
    const formData = new FormData();
    formData.append("file", file);
    const response = await fetch(
       postSensorsFetchEndPoint,
        {
            method: "POST",
            body: formData,
                  
        }
    );    
    if (!response.ok) {
        const error = new Error("Error add sensors data");
        error.message = response.status.toString();
        error.cause = response.statusText;
        console.error(error);
        return false
    }
    return true;

}
export const postSensorsFetchEndPoint = `${process.env.REACT_APP_API_URL}Sensor/UploadSensorData`