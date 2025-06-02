import LoadPage from "../pages/load_page";
import MapPage from "../pages/map_page";

export const siteConfig = {
    Links: {
        MapPage:{
            title:"Карта",
            href:"/",
            component: <MapPage/>
        },
        LoadPage:{
            title:"Загрузка данных",
            href:"load",
            component: <LoadPage/>
        }
    }
}