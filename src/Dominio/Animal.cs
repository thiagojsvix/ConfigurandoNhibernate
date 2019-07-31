using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Animal
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
    }

    public class Cachorro : Animal
    {
        public virtual Cachorro New() => new Cachorro()
        {
            Nome = "Logan",
            Pelo = "Preto"
        };

        public virtual string Pelo { get; set; }
    }

    public class Papagaio : Animal
    {
        public virtual Papagaio New() => new Papagaio()
        {
            Nome = "Papagaio Blue",
            Plumagem = "Azul"
        };

        public virtual string Plumagem { get; set; }
    }
}
