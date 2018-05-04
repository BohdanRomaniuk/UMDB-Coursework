﻿using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using website.Models.Interfaces;

namespace website.Models
{
    public class UMDBRepository: IUMDBRepository
    {
        private UMDBContext context;

        public UMDBRepository(UMDBContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Movie> Movies => context.Movies;
        public IQueryable<Genre> Genres => context.Genres;
        public IQueryable<Country> Countries => context.Countries;
        public IQueryable<MovieGenre> MovieGenres => context.MovieGenres;
        public IQueryable<MovieCountry> MovieCountries => context.MovieCountries;
        public IQueryable<User> Users => context.Users;
        public IQueryable<Comment> Comments => context.Comments;
    }
}
