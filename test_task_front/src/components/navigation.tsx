import { Link } from "react-router-dom"
import { siteConfig } from "../config/site_config"
import 'bootstrap/dist/css/bootstrap.min.css';
const Navigation = () => {
    return(
         <nav className="navbar navbar-expand-lg navbar-light bg-dark">
            <div className="container">                
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav">
                        <li className="nav-item">
                            <Link className="nav-link link-light" to={siteConfig.Links.MapPage.href}>
                                {siteConfig.Links.MapPage.title}
                            </Link>
                        </li>
                        <li className="nav-item ">
                            <Link className="nav-link link-light" to={siteConfig.Links.LoadPage.href}>
                                {siteConfig.Links.LoadPage.title}
                            </Link>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    )    
}
export default Navigation