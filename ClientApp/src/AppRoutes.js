import Aezakmi from './components/Aezakmi';
import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import SubjectView from './components/Subjects/SubjectView';

const AppRoutes = [
  {
    path: '/*',
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/subject/:id',
      requireAuth: true,
      element: <SubjectView />
    },
    {
        path: '/aezakmi',
        requireAuth: true,
        element: <Aezakmi />
    },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
