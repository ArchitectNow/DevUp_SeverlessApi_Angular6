
export interface INavigationPaths {
  path: string;
  title: string;
  authorize?: string[];
  iconType?: string;
  children?: INavigationPaths[];
  visible?: boolean;
}
