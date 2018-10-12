import {INavigationPaths} from './INavigationPaths';


export const NavigationPaths: INavigationPaths[] = [
  {
    path: '/app/wods/todays-wod',
    title: 'Today\'s Wod',
    authorize: ['wods', 'wods.readonly'],
    iconType: 'today'
  },
  {
    path: '/app/wods/list',
    title: 'Wods',
    authorize: ['wods', 'wods.readonly'],
    iconType: 'fitness_center'
  }, {
    path: '/athletes',
    title: 'Athletes',
    iconType: 'perm_identity',
    children: [{
      path: 'leads',
      title: 'Leads'
    }, {
      path: 'list',
      title: 'List'
    }]
  }, {
    path: '/classes',
    title: 'Classes',
    iconType: 'event',
    children: [{
      path: 'list',
      title: 'List'
    }, {
      path: 'locations',
      title: 'Locations',
    }]
  }, {
    path: '/app/affiliates/list',
    title: 'Affiliates',
    iconType: 'business'
  }, {
    path: '/app/financial/list',
    title: 'Financial',
    // authorize: ['financial', 'financial.readonly'],
    iconType: 'attach_money'
  }, {
    path: '/reports',
    title: 'Reports',
    iconType: 'pie_chart',
    children: [{
      path: 'list',
      title: 'List',
    }]
  }
];
