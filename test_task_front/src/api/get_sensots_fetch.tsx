export const getSensorsFetch = async () => {
    const response = await fetch(
       getSensorsFetchEndPoint,
        {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
                
            },          
        }
    );
    const data = response.json();
    if (!response.ok) {
        const error = new Error("Error fetching sensors data");
        error.message = response.status.toString();
        error.cause = response.statusText;
        throw error;
    }
    return data;

}
export const getSensorsFetchEndPoint = `${process.env.REACT_APP_API_URL}Sensor/SensorsLoacation`