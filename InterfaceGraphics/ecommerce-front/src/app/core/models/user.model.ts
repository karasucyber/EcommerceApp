export interface Usuario {
  id: number;
  nome: string;
  email: string;
  perfil: 'Administrador' | 'Operador' | 'Cliente'; 
  accessToken: string; 
}