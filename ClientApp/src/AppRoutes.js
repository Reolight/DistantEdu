import Aezakmi from './components/Aezakmi';
import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    requireAuth: true,
    element: <FetchData />
    },
    {
        path: '/aezakmi',
        requireAuth: true,
        element: <Aezakmi />
    },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
