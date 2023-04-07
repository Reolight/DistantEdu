import Aezakmi from './components/Aezakmi';
import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import LessonView from './components/Lessons/LessonView';
import QuizNew from './components/Quiz/QuizNew';
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
    {
      path: '/lesson/:params',
      requireAuth: true,
      element: <LessonView />
    },
    {
      path: '/quiz/new/:params',
      requireAuth: true,
      element: <QuizNew />
    },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
